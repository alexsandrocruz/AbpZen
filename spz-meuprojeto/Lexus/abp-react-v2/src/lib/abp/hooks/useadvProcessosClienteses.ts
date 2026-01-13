import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProcessosClientesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProcessosClienteses(input: GetAdvProcessosClientesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProcessosClienteses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-clientes", {
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

export function useAllAdvProcessosClienteses() {
  return useQuery({
    queryKey: ["advProcessosClienteses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-clientes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProcessosClientes(id: string) {
  return useQuery({
    queryKey: ["advProcessosClientes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-processos-clientes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProcessosClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-processos-clientes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosClienteses"] });
    },
  });
}

export function useUpdateAdvProcessosClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-processos-clientes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosClienteses"] });
      queryClient.invalidateQueries({ queryKey: ["advProcessosClientes", data.id] });
    },
  });
}

export function useDeleteAdvProcessosClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-processos-clientes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosClienteses"] });
    },
  });
}


