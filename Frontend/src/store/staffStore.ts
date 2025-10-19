import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import * as staffApi from '../service/staffApiService';
import type { StaffResponse, StaffAddRequest, StaffUpdateRequest, StaffSearchCriteria } from '../types/staff';

export const useStaffStore = defineStore('staff', () => {
  // State
  const staff = ref<StaffResponse[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const currentPage = ref(1);
  const pageSize = ref(100);
  const totalItems = ref(0);
  const totalPages = ref(0);
  const hasNextPage = ref(false);
  const hasPreviousPage = ref(false);

  const searchCriteria = ref<StaffSearchCriteria>({
    staffId: undefined,
    gender: undefined,
    from: undefined,
    to: undefined,
    page: 1,
    pageSize: 10,
  });

  // Getters
  const filteredStaff = computed(() => staff.value);

  const getStaffById = computed(() => (id: string) => staff.value.find((s) => s.id === id));

  // Actions
  const fetchAllStaff = async (criteria?: StaffSearchCriteria) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await staffApi.getAllStaff({
        ...searchCriteria.value,
        ...criteria,
        page: currentPage.value,
        pageSize: pageSize.value,
      });

      if (response.isSuccess) {
        staff.value = response.data;
        totalItems.value = response.totalItems;
        pageSize.value = response.pageSize;
        totalPages.value = response.totalPages;
        hasNextPage.value = response.hasNextPage;
        hasPreviousPage.value = response.hasPreviousPage;
      } else {
        error.value = response.message || 'Failed to fetch staff';
      }
    } catch (err: any) {
      error.value = err.message || 'Failed to fetch staff';
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchStaffById = async (id: string) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await staffApi.getStaffById(id);
      if (response.isSuccess) {
        return response.data;
      } else {
        error.value = response.message;
        return null;
      }
    } catch (err: any) {
      error.value = err.message || 'Failed to fetch staff by ID';
      console.error(err);
      return null;
    } finally {
      loading.value = false;
    }
  };

  const createStaff = async (data: StaffAddRequest) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await staffApi.createStaff(data);
      if (response.isSuccess) {
        staff.value.push(response.data);
        return response.data;
      } else {
        error.value = response.message;
        throw new Error(response.message);
      }
    } catch (err: any) {
      error.value = err.message || 'Failed to create staff';
      console.error(err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const updateStaff = async (id: string, data: StaffUpdateRequest) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await staffApi.updateStaff(id, data);
      if (response.isSuccess) {
        const index = staff.value.findIndex((s) => s.id === id);
        if (index !== -1) staff.value[index] = response.data;
        return response.data;
      } else {
        error.value = response.message;
        throw new Error(response.message);
      }
    } catch (err: any) {
      error.value = err.message || 'Failed to update staff';
      console.error(err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const deleteStaff = async (id: string) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await staffApi.deleteStaff(id);
      if (response.isSuccess) {
        staff.value = staff.value.filter((s) => s.id !== id);
        return true;
      } else {
        error.value = response.message;
        throw new Error(response.message);
      }
    } catch (err: any) {
      error.value = err.message || 'Failed to delete staff';
      console.error(err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const exportToPdf = async (
    pageNumber: number,
    pageSize: number,
    staffId?: string,
    gender?: number,
    from?: string,
    to?: string
  ) => {
    loading.value = true;
    error.value = null;
    try {
      const blob = await staffApi.exportToPdf(pageNumber, pageSize, staffId, gender, from, to);
      downloadFile(blob, 'staff-report.pdf', 'application/pdf');
      return true;
    } catch (err: any) {
      error.value = err.message || 'Failed to export PDF';
      console.error(err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const exportToExcel = async (
    pageNumber: number,
    pageSize: number,
    staffId?: string,
    gender?: number,
    from?: string,
    to?: string
  ) => {
    loading.value = true;
    error.value = null;
    try {
      const blob = await staffApi.exportToExcel(pageNumber, pageSize, staffId, gender, from, to);
      downloadFile(blob, 'staff-report.xlsx', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet');
      return true;
    } catch (err: any) {
      error.value = err.message || 'Failed to export Excel';
      console.error(err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const downloadFile = (fileData: Blob | ArrayBuffer, filename: string, mimeType: string) => {
    const blob = new Blob([fileData], { type: mimeType });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
  };

  const setSearchCriteria = (criteria: StaffSearchCriteria) => {
    searchCriteria.value = { ...searchCriteria.value, ...criteria };
  };

  const clearSearchCriteria = () => {
    searchCriteria.value = {
      staffId: undefined,
      gender: undefined,
      from: undefined,
      to: undefined,
      page: 1,
      pageSize: pageSize.value,
    };
    currentPage.value = 1;
  };

  const setPage = (page: number) => {
    currentPage.value = page;
    fetchAllStaff();
  };

  const nextPageAction = async () => {
    if (hasNextPage.value) {
      currentPage.value++;
      await fetchAllStaff();
    }
  };

  const previousPageAction = async () => {
    if (hasPreviousPage.value) {
      currentPage.value--;
      await fetchAllStaff();
    }
  };

  return {
    staff,
    loading,
    error,
    currentPage,
    pageSize,
    totalItems,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    searchCriteria,
    filteredStaff,
    getStaffById,
    fetchAllStaff,
    fetchStaffById,
    createStaff,
    updateStaff,
    deleteStaff,
    exportToPdf,
    exportToExcel,
    setSearchCriteria,
    clearSearchCriteria,
    setPage,
    nextPageAction,
    previousPageAction,
  };
});