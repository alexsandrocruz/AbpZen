import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesArquivosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClientesArquivoses(input: GetAdvClientesArquivosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClientesArquivoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-arquivos", {
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

export function useAllAdvClientesArquivoses() {
  return useQuery({
    queryKey: ["advClientesArquivoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes-arquivos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientesArquivos(id: string) {
  return useQuery({
    queryKey: ["advClientesArquivos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes-arquivos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientesArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes-arquivos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesArquivoses"] });
    },
  });
}

export function useUpdateAdvClientesArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes-arquivos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClientesArquivoses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientesArquivos", data.id] });
    },
  });
}

export function useDeleteAdvClientesArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes-arquivos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClientesArquivoses"] });
    },
  });
}


