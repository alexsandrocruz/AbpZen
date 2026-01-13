import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvFornecedoresesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvFornecedoreses(input: GetAdvFornecedoresesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advFornecedoreses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-fornecedores", {
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

export function useAllAdvFornecedoreses() {
  return useQuery({
    queryKey: ["advFornecedoreses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-fornecedores", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvFornecedores(id: string) {
  return useQuery({
    queryKey: ["advFornecedores", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-fornecedores/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvFornecedores() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-fornecedores", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advFornecedoreses"] });
    },
  });
}

export function useUpdateAdvFornecedores() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-fornecedores/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advFornecedoreses"] });
      queryClient.invalidateQueries({ queryKey: ["advFornecedores", data.id] });
    },
  });
}

export function useDeleteAdvFornecedores() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-fornecedores/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advFornecedoreses"] });
    },
  });
}


