import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesAtualizacoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesAtualizacoeses(input: GetAdvClientesAtualizacoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesAtualizacoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-atualizacoes", {
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

export function useAllAdvClientesAtualizacoeses() {
  return useQuery({
    queryKey: ["advClientesAtualizacoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-atualizacoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesAtualizacoes(id: string) {
  return useQuery({
    queryKey: ["advClientesAtualizacoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-atualizacoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesAtualizacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-atualizacoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesAtualizacoeses"] });
    },
  });
}

export function useUpdateAdvClientesAtualizacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-atualizacoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesAtualizacoeses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesAtualizacoes", data.id] });
    },
  });
}

export function useDeleteAdvClientesAtualizacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-atualizacoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesAtualizacoeses"] });
    },
  });
}


