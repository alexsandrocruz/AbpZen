import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliLogsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliLogs(input: GetAdvCliLogsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliLogs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-log", {
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

export function useAllAdvCliLogs() {
  return useQuery({
    queryKey: ["advCliLogs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-log", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliLog(id: string) {
  return useQuery({
    queryKey: ["advCliLog", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-log/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-log", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliLogs"] });
    },
  });
}

export function useUpdateAdvCliLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-log/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliLogs"] });
      queryClient.invalidateQueries({ queryKey: ["advCliLog", data.id] });
    },
  });
}

export function useDeleteAdvCliLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-log/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliLogs"] });
    },
  });
}


