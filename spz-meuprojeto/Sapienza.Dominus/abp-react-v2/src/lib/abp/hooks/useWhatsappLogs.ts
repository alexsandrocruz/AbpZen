import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetWhatsappLogsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useWhatsappLogs(input: GetWhatsappLogsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["whatsappLogs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/whatsapp-log", {
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

export function useAllWhatsappLogs() {
  return useQuery({
    queryKey: ["whatsappLogs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/whatsapp-log", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useWhatsappLog(id: string) {
  return useQuery({
    queryKey: ["whatsappLog", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/whatsapp-log/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateWhatsappLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/whatsapp-log", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["whatsappLogs"] });
    },
  });
}

export function useUpdateWhatsappLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/whatsapp-log/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["whatsappLogs"] });
      queryClient.invalidateQueries({ queryKey: ["whatsappLog", data.id] });
    },
  });
}

export function useDeleteWhatsappLog() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/whatsapp-log/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["whatsappLogs"] });
    },
  });
}


