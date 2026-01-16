import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabCidadesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabCidadeses(input: GetFabCidadesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabCidadeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-cidades", {
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

export function useAllFabCidadeses() {
  return useQuery({
    queryKey: ["fabCidadeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-cidades", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabCidades(id: string) {
  return useQuery({
    queryKey: ["fabCidades", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-cidades/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabCidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-cidades", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabCidadeses"] });
    },
  });
}

export function useUpdateFabCidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-cidades/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabCidadeses"] });
      queryClient.invalidateQueries({ queryKey: ["fabCidades", data.id] });
    },
  });
}

export function useDeleteFabCidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-cidades/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabCidadeses"] });
    },
  });
}


