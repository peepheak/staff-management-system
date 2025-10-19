
import axios from 'axios';

export const apiClient = axios.create({
    baseURL: 'http://localhost:5000/api/v1',
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
    },
});


apiClient.interceptors.request.use(
    (config) => {

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response) {
            switch (error.response.status) {
                case 401:
                    console.error('Unauthorized access');
                    break;
                case 403:
                    console.error('Access forbidden');
                    break;
                case 404:
                    console.error('Resource not found');
                    break;
                case 500:
                    console.error('Server error');
                    break;
                default:
                    console.error(`HTTP Error ${error.response.status}`);
                    break;
            }
        } else if (error.request) {
            console.error('No response received from server.');
        } else {
            console.error('Request setup error:', error.message);
        }
        return Promise.reject(error);
    }
);

export default apiClient;