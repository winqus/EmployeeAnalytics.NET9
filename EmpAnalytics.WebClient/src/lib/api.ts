import type {
  RecentlyActiveEmployeeResponse,
  AboveAveragePerformerResponse,
  EmployeeExceedingOwnAverageResponse,
  AboveAverageFilters,
} from './types';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5112';
const EMPLOYEES_BASE_URL = API_URL + '/api/v1/employees';

export const api = {
  employees: {
    getRecentlyActive: async (): Promise<RecentlyActiveEmployeeResponse[]> => {
      const res = await fetch(`${EMPLOYEES_BASE_URL}/recently-active`);
      if (!res.ok) {
        throw new Error('Failed to fetch recently active employees');
      }

      return res.json();
    },

    getAboveAveragePerformers: async (
      params?: AboveAverageFilters
    ): Promise<AboveAveragePerformerResponse[]> => {
      const searchParams = new URLSearchParams();
      if (params?.startDate) {
        searchParams.set('startDate', params.startDate);
      }
      if (params?.endDate) {
        searchParams.set('endDate', params.endDate);
      }

      const query = searchParams.toString();
      const res = await fetch(
        `${EMPLOYEES_BASE_URL}/above-average${query ? `?${query}` : ''}`
      );
      if (!res.ok) {
        throw new Error('Failed to fetch above average performers');
      }

      return res.json();
    },

    getExceedingOwnAverage: async (): Promise<EmployeeExceedingOwnAverageResponse[]> => {
      const res = await fetch(`${EMPLOYEES_BASE_URL}/exceeding-own-average`);
      if (!res.ok) {
        throw new Error('Failed to fetch employees exceeding own average');
      }

      return res.json();
    },
  },
};
