<template>
    <div class="bg-white rounded-lg shadow-md p-6 max-w-md mx-auto">
        <form @submit.prevent="handleSubmit" class="space-y-4">
            <!-- Staff ID -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">
                    Staff ID <span class="text-red-500">*</span>
                </label>
                <input v-model="formData.staffId" type="text" maxlength="8"
                    class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent disabled:bg-gray-100 disabled:cursor-not-allowed"
                    placeholder="e.g., STF00001" />
                <p v-if="errors.staffId" class="text-red-500 text-sm mt-1">{{ errors.staffId }}</p>
            </div>

            <!-- Full Name -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">
                    Full Name <span class="text-red-500">*</span>
                </label>
                <input v-model="formData.fullName" type="text" maxlength="100"
                    class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent"
                    placeholder="Enter full name" />
                <p v-if="errors.fullName" class="text-red-500 text-sm mt-1">{{ errors.fullName }}</p>
            </div>

            <!-- Birthday -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">
                    Birthday <span class="text-red-500">*</span>
                </label>
                <input v-model="formData.birthday" type="date"
                    class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent" />
                <p v-if="errors.birthday" class="text-red-500 text-sm mt-1">{{ errors.birthday }}</p>
            </div>

            <!-- Gender -->
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">
                    Gender <span class="text-red-500">*</span>
                </label>
                <select v-model="formData.gender"
                    class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent">
                    <option :value="1">Male</option>
                    <option :value="2">Female</option>
                </select>
            </div>

            <!-- Buttons -->
            <div class="flex gap-3 mt-6">
                <button type="submit" :disabled="loading || !isValid"
                    class="flex-1 bg-indigo-600 text-white py-3 rounded-lg hover:bg-indigo-700 transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed">
                    <span v-if="loading">Processing...</span>
                    <span v-else>{{ isEdit ? 'Update' : 'Create' }} Staff</span>
                </button>
                <button type="button" @click="handleCancel" :disabled="loading"
                    class="flex-1 bg-gray-200 text-gray-700 py-3 rounded-lg hover:bg-gray-300 transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed">
                    Cancel
                </button>
            </div>
        </form>
    </div>
</template>

<script setup lang="ts">
import { reactive, watch, computed } from 'vue';
import type { StaffResponse } from '../types/staff';

interface FormData {
    staffId: string;
    fullName: string;
    birthday: string;
    gender: number;
}

interface FormErrors {
    staffId?: string;
    fullName?: string;
    birthday?: string;
}

const props = defineProps<{
    initialData?: StaffResponse | null;
    isEdit?: boolean;
    loading?: boolean;
}>();

const emit = defineEmits<{
    (e: 'submit', data: FormData): void;
    (e: 'cancel'): void;
}>();

const formData = reactive<FormData>({
    staffId: '',
    fullName: '',
    birthday: '',
    gender: 1,
});

const errors = reactive<FormErrors>({});

// Populate form if editing
watch(
    () => props.initialData,
    (newData) => {
        if (newData) {
            formData.staffId = newData.staffId;
            formData.fullName = newData.fullName;
            formData.birthday = new Date(newData.birthday).toISOString().split('T')[0];
            formData.gender = newData.gender;
        }
    },
    { immediate: true }
);

// Validation
const validate = (): boolean => {
    errors.staffId = !formData.staffId ? 'Staff ID is required.' : undefined;
    errors.fullName = !formData.fullName ? 'Full Name is required.' : undefined;
    errors.birthday = !formData.birthday ? 'Birthday is required.' : undefined;

    return !errors.staffId && !errors.fullName && !errors.birthday;
};

const isValid = computed(() => validate());

const handleSubmit = () => {
    if (!validate()) return;
    emit('submit', { ...formData });
};

const handleCancel = () => {
    emit('cancel');
};
</script>