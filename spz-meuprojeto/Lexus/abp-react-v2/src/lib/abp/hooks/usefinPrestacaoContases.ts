import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinPrestacaoContasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinPrestacaoContases(input: GetFinPrestacaoContasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finPrestacaoContases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-prestacao-contas", {
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

export function useAllFinPrestacaoContases() {
  return useQuery({
    queryKey: ["finPrestacaoContases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-prestacao-contas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinPrestacaoContas(id: string) {
  return useQuery({
    queryKey: ["finPrestacaoContas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-prestacao-contas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinPrestacaoContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-prestacao-contas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPrestacaoContases"] });
    },
  });
}

export function useUpdateFinPrestacaoContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-prestacao-contas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finPrestacaoContases"] });
      queryClient.invalidateQueries({ queryKey: ["finPrestacaoContas", data.id] });
    },
  });
}

export function useDeleteFinPrestacaoContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-prestacao-contas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPrestacaoContases"] });
    },
  });
}


