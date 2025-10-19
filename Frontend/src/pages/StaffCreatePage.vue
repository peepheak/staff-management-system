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

        <h2 class="text-3xl font-bold text-gray-800 mb-6">Create New Staff</h2>

        <div class="bg-gray-50 p-6 rounded-lg mb-6 border border-gray-200">
            <h3 class="text-lg font-semibold mb-4 text-gray-700">Staff Details</h3>
            <StaffForm :loading="staffStore.loading" @submit="handleSubmit" @cancel="handleCancel" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useStaffStore } from '../store/staffStore';
import StaffForm from '../components/StaffForm.vue';
import type { StaffAddRequest } from '../types/staff';

const router = useRouter();
const staffStore = useStaffStore();

const formatDateToString = (date: Date | string): string => {
    if (!date) return '';
    
    const d = typeof date === 'string' ? new Date(date) : date;
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    const year = d.getFullYear();
    
    return `${year}-${month}-${day}`;
};

const handleSubmit = async (formData: any) => {
    try {
        const staffData: StaffAddRequest = {
            staffId: formData.staffId,
            fullName: formData.fullName,
            birthday: formatDateToString(formData.birthday),
            gender: parseInt(formData.gender),
        };

        await staffStore.createStaff(staffData);
        alert('Staff created successfully!');
        router.push('/');
    } catch (error: any) {
        alert(error.message || 'Failed to create staff');
    }
};

const handleCancel = () => {
    router.push('/');
};
</script>