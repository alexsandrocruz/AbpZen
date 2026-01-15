import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSchedulerExceptionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  schedulerTypeId?: string;
  }

export function useSchedulerExceptions(input: GetSchedulerExceptionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["schedulerExceptions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/scheduler-exception", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllSchedulerExceptions() {
  return useQuery({
    queryKey: ["schedulerExceptions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/scheduler-exception", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSchedulerException(id: string) {
  return useQuery({
    queryKey: ["schedulerException", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/scheduler-exception/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSchedulerException() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/scheduler-exception", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedulerExceptions"] });
    },
  });
}

export function useUpdateSchedulerException() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/scheduler-exception/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["schedulerExceptions"] });
      queryClient.invalidateQueries({ queryKey: ["schedulerException", data.id] });
    },
  });
}

export function useDeleteSchedulerException() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/scheduler-exception/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedulerExceptions"] });
    },
  });
}


