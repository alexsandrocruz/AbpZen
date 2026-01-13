import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinExtratosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinExtratos(input: GetFinExtratosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finExtratos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-extrato", {
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

export function useAllFinExtratos() {
  return useQuery({
    queryKey: ["finExtratos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-extrato", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinExtrato(id: string) {
  return useQuery({
    queryKey: ["finExtrato", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-extrato/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinExtrato() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-extrato", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finExtratos"] });
    },
  });
}

export function useUpdateFinExtrato() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-extrato/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finExtratos"] });
      queryClient.invalidateQueries({ queryKey: ["finExtrato", data.id] });
    },
  });
}

export function useDeleteFinExtrato() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-extrato/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finExtratos"] });
    },
  });
}


