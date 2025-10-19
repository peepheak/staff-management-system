<template>
  <div>
    <!-- Header -->
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-3xl font-bold text-gray-800">Staff List</h2>
      <button @click="router.push('/create')"
        class="flex items-center gap-2 bg-indigo-600 text-white px-6 py-3 rounded-lg hover:bg-indigo-700 transition-colors shadow-lg">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2">
          <path d="M5 12h14"></path>
          <path d="M12 5v14"></path>
        </svg>
        Add Staff
      </button>
    </div>

    <!-- Advanced Search Toggle -->
    <div class="mb-6">
      <button @click="showAdvSearch = !showAdvSearch"
        class="flex items-center gap-2 bg-gray-100 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-200 transition-colors">
        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none"
          stroke="currentColor" stroke-width="2">
          <circle cx="11" cy="11" r="8"></circle>
          <path d="m21 21-4.3-4.3"></path>
        </svg>
        Advanced Search
      </button>
    </div>

    <!-- Advanced Search Panel -->
    <div v-if="showAdvSearch" class="bg-gray-50 p-6 rounded-lg mb-6 border border-gray-200">
      <h3 class="text-lg font-semibold mb-4 text-gray-700">Search Criteria</h3>
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Staff ID</label>
          <input v-model="searchCriteria.staffId" type="text"
            class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
            placeholder="Search by ID" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Gender</label>
          <select v-model="searchCriteria.gender"
            class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent">
            <option :value="undefined">All</option>
            <option :value="1">Male</option>
            <option :value="2">Female</option>
          </select>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Birthday From</label>
          <input v-model="searchCriteria.from" type="date"
            class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent" />
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Birthday To</label>
          <input v-model="searchCriteria.to" type="date"
            class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent" />
        </div>
      </div>
      <div class="flex gap-3 mt-4">
        <button @click="handleSearch"
          class="px-4 py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition-colors">
          Search
        </button>
        <button @click="handleClearFilters"
          class="px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300 transition-colors">
          Clear Filters
        </button>
      </div>
    </div>

    <!-- Export Buttons -->
    <div class="flex gap-3 mb-6">
      <button @click="handleExportExcel" :disabled="staffStore.loading"
        class="flex items-center gap-2 bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed">
        Export Excel
      </button>
      <button @click="handleExportPDF" :disabled="staffStore.loading"
        class="flex items-center gap-2 bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed">
        Export PDF
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="staffStore.loading" class="text-center py-12">
      <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      <p class="mt-4 text-gray-600">Loading staff data...</p>
    </div>

    <!-- Staff Table -->
    <StaffTable v-else :data="staffStore.filteredStaff" @edit="handleEdit" @delete="handleDelete" />

    <!-- Pagination
    <div v-if="staffStore.totalPages > 1" class="flex justify-center items-center gap-4 mt-6">
      <button @click="handlePreviousPage" :disabled="!staffStore.hasPreviousPage"
        class="px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300 transition-colors disabled:opacity-50 disabled:cursor-not-allowed">
        Previous
      </button>
      <span class="text-gray-700">
        Page {{ staffStore.currentPage }} of {{ staffStore.totalPages }}
      </span>
      <button @click="handleNextPage" :disabled="!staffStore.hasNextPage"
        class="px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300 transition-colors disabled:opacity-50 disabled:cursor-not-allowed">
        Next
      </button>
    </div> -->

  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useStaffStore } from '../store/staffStore';
import StaffTable from '../components/StaffTable.vue';

const router = useRouter();
const staffStore = useStaffStore();
const showAdvSearch = ref(false);

const searchCriteria = ref({
  staffId: undefined,
  gender: undefined,
  from: undefined,
  to: undefined,
});

onMounted(async () => {
  await staffStore.fetchAllStaff(searchCriteria.value);
  console.log('Initial staff data:', staffStore.filteredStaff);
});

const handleSearch = async () => {
  staffStore.setSearchCriteria(searchCriteria.value);
  await staffStore.fetchAllStaff(searchCriteria.value);
};

const handleClearFilters = async () => {
  searchCriteria.value = {
    staffId: undefined,
    gender: undefined,
    from: undefined,
    to: undefined,
  };
  staffStore.clearSearchCriteria();
  await staffStore.fetchAllStaff();
};

const handleEdit = (id: string) => router.push(`/edit/${id}`);

const handleDelete = async (id: string) => {
  if (confirm('Are you sure you want to delete this staff member?')) {
    try {
      await staffStore.deleteStaff(id);
      alert('Staff deleted successfully!');
      await staffStore.fetchAllStaff(searchCriteria.value);
    } catch (err) {
      alert('Failed to delete staff');
      console.error(err);
    }
  }
};

const handleExportExcel = async () => {
  try {
    await staffStore.exportToExcel(
      staffStore.currentPage,
      staffStore.pageSize,
      searchCriteria.value.staffId,
      searchCriteria.value.gender,
      searchCriteria.value.from,
      searchCriteria.value.to
    );
    alert('Excel exported successfully!');
  } catch (err) {
    alert('Failed to export Excel');
    console.error(err);
  }
};

const handleExportPDF = async () => {
  try {
    await staffStore.exportToPdf(
      staffStore.currentPage,
      staffStore.pageSize,
      searchCriteria.value.staffId,
      searchCriteria.value.gender,
      searchCriteria.value.from,
      searchCriteria.value.to
    );
    alert('PDF exported successfully!');
  } catch (err) {
    alert('Failed to export PDF');
    console.error(err);
  }
};

const handlePreviousPage = async () => {
  await staffStore.previousPageAction();
};

const handleNextPage = async () => {
  await staffStore.nextPageAction();
};
</script>