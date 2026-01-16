import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinContasClientesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinContasClienteses(input: GetFinContasClientesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finContasClienteses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-contas-clientes", {
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

export function useAllFinContasClienteses() {
  return useQuery({
    queryKey: ["finContasClienteses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-contas-clientes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinContasClientes(id: string) {
  return useQuery({
    queryKey: ["finContasClientes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-contas-clientes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinContasClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-contas-clientes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finContasClienteses"] });
    },
  });
}

export function useUpdateFinContasClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-contas-clientes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finContasClienteses"] });
      queryClient.invalidateQueries({ queryKey: ["finContasClientes", data.id] });
    },
  });
}

export function useDeleteFinContasClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-contas-clientes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finContasClienteses"] });
    },
  });
}


