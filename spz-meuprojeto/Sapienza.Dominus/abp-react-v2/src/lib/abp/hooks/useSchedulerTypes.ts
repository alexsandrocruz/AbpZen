import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSchedulerTypesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useSchedulerTypes(input: GetSchedulerTypesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["schedulerTypes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/scheduler-type", {
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

export function useAllSchedulerTypes() {
  return useQuery({
    queryKey: ["schedulerTypes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/scheduler-type", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSchedulerType(id: string) {
  return useQuery({
    queryKey: ["schedulerType", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/scheduler-type/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSchedulerType() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/scheduler-type", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedulerTypes"] });
    },
  });
}

export function useUpdateSchedulerType() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/scheduler-type/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["schedulerTypes"] });
      queryClient.invalidateQueries({ queryKey: ["schedulerType", data.id] });
    },
  });
}

export function useDeleteSchedulerType() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/scheduler-type/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedulerTypes"] });
    },
  });
}


