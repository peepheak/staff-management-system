import apiClient from '../api/axios';
import type { BaseResponse, PagedResponse } from '../types/base/base';
import type { StaffResponse, StaffAddRequest, StaffUpdateRequest, StaffSearchCriteria } from '../types/staff';

export const getAllStaff = async (
    criteria?: StaffSearchCriteria
): Promise<PagedResponse<StaffResponse>> => {
    try {
        const response = await apiClient.get<PagedResponse<StaffResponse>>(
            '/staffs',
            { params: criteria }
        );
        return response.data;
    } catch (error) {
        console.error('Error fetching staff:', error);
        throw error;
    }
};

export const getStaffById = async (
    id: string
): Promise<BaseResponse<StaffResponse>> => {
    try {
        const response = await apiClient.get<BaseResponse<StaffResponse>>(
            `${'/staff'}/${id}`
        );
        return response.data;
    } catch (error) {
        console.error('Error fetching staff by ID:', error);
        throw error;
    }
};

export const createStaff = async (
    data: StaffAddRequest
): Promise<BaseResponse<StaffResponse>> => {
    try {
        const response = await apiClient.post<BaseResponse<StaffResponse>>(
            '/staff',
            data
        );
        return response.data;
    } catch (error) {
        console.error('Error creating staff:', error);
        throw error;
    }
};

export const updateStaff = async (
    id: string,
    data: StaffUpdateRequest
): Promise<BaseResponse<StaffResponse>> => {
    try {
        const response = await apiClient.put<BaseResponse<StaffResponse>>(
            '/staff',
            data
        );
        return response.data;
    } catch (error) {
        console.error('Error updating staff:', error);
        throw error;
    }
};

export const deleteStaff = async (
    id: string
): Promise<BaseResponse<null>> => {
    try {
        const response = await apiClient.delete<BaseResponse<null>>(
            `${'/staff'}/${id}`
        );
        return response.data;
    } catch (error) {
        console.error('Error deleting staff:', error);
        throw error;
    }
};

export const exportToPdf = async (
    pageNumber: number,
    pageSize: number,
    staffId?: string,
    gender?: number,
    from?: string,
    to?: string
): Promise<Blob> => {
    try {
        const response = await apiClient.get('/staffs/pdf', {
            params: {
                pageNumber,
                pageSize,
                staffId,
                gender,
                from,
                to,
            },
            responseType: 'blob',
        });
        return response.data;
    } catch (error) {
        console.error('Error exporting PDF:', error);
        throw error;
    }
};

export const exportToExcel = async (
    pageNumber: number,
    pageSize: number,
    staffId?: string,
    gender?: number,
    from?: string,
    to?: string
): Promise<Blob> => {
    try {
        const response = await apiClient.get('/staffs/excel', {
            params: {
                pageNumber,
                pageSize,
                staffId,
                gender,
                from,
                to,
            },
            responseType: 'blob',
        });
        return response.data;
    } catch (error) {
        console.error('Error exporting Excel:', error);
        throw error;
    }
};