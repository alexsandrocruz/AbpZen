import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvClientesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvClienteses(input: GetAdvClientesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advClienteses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes", {
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

export function useAllAdvClienteses() {
  return useQuery({
    queryKey: ["advClienteses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-clientes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvClientes(id: string) {
  return useQuery({
    queryKey: ["advClientes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-clientes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-clientes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClienteses"] });
    },
  });
}

export function useUpdateAdvClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-clientes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advClienteses"] });
      queryClient.invalidateQueries({ queryKey: ["advClientes", data.id] });
    },
  });
}

export function useDeleteAdvClientes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-clientes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advClienteses"] });
    },
  });
}


