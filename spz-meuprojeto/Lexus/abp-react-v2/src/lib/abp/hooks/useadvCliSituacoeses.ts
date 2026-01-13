import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliSituacoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliSituacoeses(input: GetAdvCliSituacoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliSituacoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-situacoes", {
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

export function useAllAdvCliSituacoeses() {
  return useQuery({
    queryKey: ["advCliSituacoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-situacoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliSituacoes(id: string) {
  return useQuery({
    queryKey: ["advCliSituacoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-situacoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliSituacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-situacoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliSituacoeses"] });
    },
  });
}

export function useUpdateAdvCliSituacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-situacoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliSituacoeses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliSituacoes", data.id] });
    },
  });
}

export function useDeleteAdvCliSituacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-situacoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliSituacoeses"] });
    },
  });
}


