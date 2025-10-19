<template>
    <div class="overflow-x-auto">
        <table class="w-full">
            <thead>
                <tr class="bg-indigo-600 text-white">
                    <th class="px-6 py-4 text-left text-sm font-semibold">Staff ID</th>
                    <th class="px-6 py-4 text-left text-sm font-semibold">Full Name</th>
                    <th class="px-6 py-4 text-left text-sm font-semibold">Birthday</th>
                    <th class="px-6 py-4 text-left text-sm font-semibold">Gender</th>
                    <th class="px-6 py-4 text-left text-sm font-semibold">Created At</th>
                    <th class="px-6 py-4 text-left text-sm font-semibold">Updated At</th>
                    <th class="px-6 py-4 text-center text-sm font-semibold">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(staff, index) in data" :key="staff.id" :class="index % 2 === 0 ? 'bg-gray-50' : 'bg-white'">
                    <td class="px-6 py-4 text-sm text-gray-800">{{ staff.staffId }}</td>
                    <td class="px-6 py-4 text-sm text-gray-800">{{ staff.fullName }}</td>
                    <td class="px-6 py-4 text-sm text-gray-800">{{ formatDate(staff.birthday) }}</td>
                    <td class="px-6 py-4 text-sm text-gray-800">
                        <span :class="[
                            'px-3 py-1 rounded-full text-xs font-medium',
                            staff.gender === 1 ? 'bg-blue-100 text-blue-700' : 'bg-pink-100 text-pink-700',
                        ]">
                            {{ staff.gender === 1 ? 'Male' : 'Female' }}
                        </span>
                    </td>
                    <td class="px-6 py-4 text-sm text-gray-800">{{ formatDateTime(staff.createdAt) }}</td>
                    <td class="px-6 py-4 text-sm text-gray-800">{{ formatDateTime(staff.updatedAt) }}</td>
                    <td class="px-6 py-4 text-center">
                        <div class="flex justify-center gap-2">
                            <button @click="$emit('edit', staff.id)"
                                class="p-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition-colors"
                                title="Edit">Edit</button>
                            <button @click="$emit('delete', staff.id)"
                                class="p-2 bg-red-500 text-white rounded-lg hover:bg-red-600 transition-colors"
                                title="Delete">Delete</button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div v-if="data.length === 0" class="text-center py-12 text-gray-500">
            No staff members found
        </div>
    </div>
</template>

<script setup lang="ts">
import type { StaffResponse } from '../types/staff';

defineProps<{
    data: StaffResponse[];
}>();

defineEmits<{
    edit: [id: string];
    delete: [id: string];
}>();


const formatDateTime = (isoDate: string | null | undefined): string => {
    if (!isoDate) return '-';

    try {
        const date = new Date(isoDate);

        if (isNaN(date.getTime())) {
            return '-';
        }

        const day = String(date.getDate()).padStart(2, '0');
        const month = date.toLocaleString('en-US', { month: 'short' });
        const year = date.getFullYear();
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');

        return `${day} ${month} ${year}, ${hours}:${minutes}`;
    } catch (error) {
        console.error('Error formatting datetime:', error);
        return '-';
    }
};
const formatDate = (isoDate: string | null | undefined): string => {
    if (!isoDate) return '-';

    try {
        const date = new Date(isoDate);

        if (isNaN(date.getTime())) {
            return '-';
        }

        const day = String(date.getDate()).padStart(2, '0');
        const month = date.toLocaleString('en-US', { month: 'short' });
        const year = date.getFullYear();

        return `${day} ${month} ${year}`;
    } catch (error) {
        console.error('Error formatting date:', error);
        return '-';
    }
};
</script>