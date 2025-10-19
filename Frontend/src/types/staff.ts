export interface StaffResponse {
    id: string;
    staffId: string;
    fullName: string;
    birthday: string;
    gender: number;
    createdAt: string;
    updatedAt: string;
}

export interface StaffAddRequest {
    staffId: string;
    fullName: string;
    birthday: string;
    gender: number;
}

export interface StaffUpdateRequest {
    id: string;
    staffId: string;
    fullName: string;
    birthday: string;
    gender: number;
}

export interface StaffSearchCriteria {
    staffId?: string;
    gender?: number;
    from?: Date;
    to?: Date;
    page?: number;
    pageSize?: number;
}