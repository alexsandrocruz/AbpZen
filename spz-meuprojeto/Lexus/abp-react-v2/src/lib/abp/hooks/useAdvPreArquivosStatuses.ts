import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreArquivosStatusesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreArquivosStatuses(input: GetAdvPreArquivosStatusesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreArquivosStatuses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-arquivos-status", {
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

export function useAllAdvPreArquivosStatuses() {
  return useQuery({
    queryKey: ["advPreArquivosStatuses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-arquivos-status", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreArquivosStatus(id: string) {
  return useQuery({
    queryKey: ["advPreArquivosStatus", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-arquivos-status/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreArquivosStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-arquivos-status", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreArquivosStatuses"] });
    },
  });
}

export function useUpdateAdvPreArquivosStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-arquivos-status/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreArquivosStatuses"] });
      queryClient.invalidateQueries({ queryKey: ["advPreArquivosStatus", data.id] });
    },
  });
}

export function useDeleteAdvPreArquivosStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-arquivos-status/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreArquivosStatuses"] });
    },
  });
}


