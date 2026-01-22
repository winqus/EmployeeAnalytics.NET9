import { useQuery } from '@tanstack/react-query';
import { api } from './api';
import type { AboveAverageFilters } from './types';

export const employeeQueryKeys = {
  all: ['employees'] as const,
  recentlyActive: () => [...employeeQueryKeys.all, 'recently-active'] as const,
  aboveAverage: () => [...employeeQueryKeys.all, 'above-average'] as const,
  aboveAverageFiltered: (filters: AboveAverageFilters) =>
    [...employeeQueryKeys.aboveAverage(), filters] as const,
  exceedingOwnAverage: () => [...employeeQueryKeys.all, 'exceeding-own-average'] as const,
};

export function useRecentlyActiveEmployeesQuery() {
  return useQuery({
    queryKey: employeeQueryKeys.recentlyActive(),
    queryFn: () => api.employees.getRecentlyActive(),
  });
}

export function useAboveAveragePerformersQuery(filters?: AboveAverageFilters) {
  return useQuery({
    queryKey: filters
      ? employeeQueryKeys.aboveAverageFiltered(filters)
      : employeeQueryKeys.aboveAverage(),
    queryFn: () => api.employees.getAboveAveragePerformers(filters),
  });
}

export function useExceedingOwnAverageQuery() {
  return useQuery({
    queryKey: employeeQueryKeys.exceedingOwnAverage(),
    queryFn: () => api.employees.getExceedingOwnAverage(),
  });
}
