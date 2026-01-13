import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinPlanoContasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinPlanoContases(input: GetFinPlanoContasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finPlanoContases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-plano-contas", {
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

export function useAllFinPlanoContases() {
  return useQuery({
    queryKey: ["finPlanoContases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-plano-contas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinPlanoContas(id: string) {
  return useQuery({
    queryKey: ["finPlanoContas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-plano-contas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinPlanoContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-plano-contas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContases"] });
    },
  });
}

export function useUpdateFinPlanoContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-plano-contas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContases"] });
      queryClient.invalidateQueries({ queryKey: ["finPlanoContas", data.id] });
    },
  });
}

export function useDeleteFinPlanoContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-plano-contas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContases"] });
    },
  });
}


