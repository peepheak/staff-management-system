<template>
    <div>
        <div class="mb-6">
            <button @click="$router.push('/')"
                class="flex items-center gap-2 text-gray-600 hover:text-gray-800 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                    stroke="currentColor" stroke-width="2">
                    <path d="m15 18-6-6 6-6"></path>
                </svg>
                Back to List
            </button>
        </div>

        <h2 class="text-3xl font-bold text-gray-800 mb-6">Edit Staff</h2>

        <div class="bg-gray-50 p-6 rounded-lg mb-6 border border-gray-200">
            <h3 class="text-lg font-semibold mb-4 text-gray-700">Staff Details</h3>
            
            <div v-if="loading" class="text-center py-6">
                <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
                <p class="mt-4 text-gray-600">Loading staff data...</p>
            </div>

            <div v-else-if="!staffData" class="bg-red-50 border border-red-200 rounded-lg p-4">
                <p class="text-red-800">Staff member not found</p>
                <button @click="$router.push('/')"
                    class="mt-4 bg-gray-200 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-300">
                    Back to List
                </button>
            </div>

            <StaffForm v-else :initial-data="staffData" :is-edit="true" :loading="staffStore.loading" @submit="handleSubmit"
                @cancel="handleCancel" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useStaffStore } from '../store/staffStore';
import StaffForm from '../components/StaffForm.vue';
import type { StaffResponse, StaffUpdateRequest } from '../types/staff';

const router = useRouter();
const route = useRoute();
const staffStore = useStaffStore();

// State
const staffData = ref<StaffResponse | null>(null);
const loading = ref(true);

// Fetch staff data on mount
onMounted(async () => {
    const id = route.params.id as string;
    if (id) {
        try {
            staffData.value = await staffStore.fetchStaffById(id);
        } catch (error) {
            console.error('Error loading staff:', error);
        } finally {
            loading.value = false;
        }
    }
});

const formatDateToString = (date: Date | string): string => {
    if (!date) return '';

    const d = typeof date === 'string' ? new Date(date) : date;
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    const year = d.getFullYear();

    return `${year}-${month}-${day}`;
};

// Handle form submit
const handleSubmit = async (formData: any) => {
    const id = route.params.id as string;

    if (!id) return;

    const updateData: StaffUpdateRequest = {
        id,
        staffId: formData.staffId,
        fullName: formData.fullName,
        birthday: formatDateToString(formData.birthday),
        gender: parseInt(formData.gender),
    };

    try {
        await staffStore.updateStaff(id, updateData);
        alert('Staff updated successfully!');
        router.push('/');
    } catch (error: any) {
        alert(error.message || 'Failed to update staff');
    }
};

// Handle cancel
const handleCancel = () => {
    router.push('/');
};
</script>