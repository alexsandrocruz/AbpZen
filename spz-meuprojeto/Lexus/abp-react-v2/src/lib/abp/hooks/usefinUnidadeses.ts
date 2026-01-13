import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinUnidadesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinUnidadeses(input: GetFinUnidadesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finUnidadeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-unidades", {
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

export function useAllFinUnidadeses() {
  return useQuery({
    queryKey: ["finUnidadeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-unidades", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinUnidades(id: string) {
  return useQuery({
    queryKey: ["finUnidades", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-unidades/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinUnidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-unidades", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finUnidadeses"] });
    },
  });
}

export function useUpdateFinUnidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-unidades/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finUnidadeses"] });
      queryClient.invalidateQueries({ queryKey: ["finUnidades", data.id] });
    },
  });
}

export function useDeleteFinUnidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-unidades/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finUnidadeses"] });
    },
  });
}


