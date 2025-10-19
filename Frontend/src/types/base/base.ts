export interface BaseResponse<T = any> {
    data: T;
    message: string;
    statusCode: number;
    isSuccess: boolean;
}

export interface PagedResponse<T = any> extends BaseResponse<T[]> {
    currentPage: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}