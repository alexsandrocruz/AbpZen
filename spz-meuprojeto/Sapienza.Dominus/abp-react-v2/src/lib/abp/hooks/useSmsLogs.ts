import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSmsLogsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useSmsLogs(input: GetSmsLogsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["smsLogs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/sms-log", {
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

export function useAllSmsLogs() {
  return useQuery({
    queryKey: ["smsLogs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/sms-log", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSmsLog(id: string) {
  return useQuery({
    queryKey: ["smsLog", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/sms-log/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSmsLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/sms-log", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["smsLogs"] });
    },
  });
}

export function useUpdateSmsLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/sms-log/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["smsLogs"] });
      queryClient.invalidateQueries({ queryKey: ["smsLog", data.id] });
    },
  });
}

export function useDeleteSmsLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/sms-log/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["smsLogs"] });
    },
  });
}


