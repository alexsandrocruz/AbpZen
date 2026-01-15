import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetEmailLogsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useEmailLogs(input: GetEmailLogsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["emailLogs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/email-log", {
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

export function useAllEmailLogs() {
  return useQuery({
    queryKey: ["emailLogs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/email-log", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useEmailLog(id: string) {
  return useQuery({
    queryKey: ["emailLog", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/email-log/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateEmailLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/email-log", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["emailLogs"] });
    },
  });
}

export function useUpdateEmailLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/email-log/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["emailLogs"] });
      queryClient.invalidateQueries({ queryKey: ["emailLog", data.id] });
    },
  });
}

export function useDeleteEmailLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/email-log/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["emailLogs"] });
    },
  });
}


