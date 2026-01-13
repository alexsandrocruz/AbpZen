import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinPlanoContasDetsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinPlanoContasDets(input: GetFinPlanoContasDetsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finPlanoContasDets", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-plano-contas-det", {
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

export function useAllFinPlanoContasDets() {
  return useQuery({
    queryKey: ["finPlanoContasDets", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-plano-contas-det", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinPlanoContasDet(id: string) {
  return useQuery({
    queryKey: ["finPlanoContasDet", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-plano-contas-det/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinPlanoContasDet() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-plano-contas-det", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasDets"] });
    },
  });
}

export function useUpdateFinPlanoContasDet() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-plano-contas-det/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasDets"] });
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasDet", data.id] });
    },
  });
}

export function useDeleteFinPlanoContasDet() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-plano-contas-det/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasDets"] });
    },
  });
}


