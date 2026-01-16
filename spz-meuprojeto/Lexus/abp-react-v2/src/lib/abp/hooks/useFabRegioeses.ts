import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabRegioesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabRegioeses(input: GetFabRegioesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabRegioeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-regioes", {
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

export function useAllFabRegioeses() {
  return useQuery({
    queryKey: ["fabRegioeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-regioes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabRegioes(id: string) {
  return useQuery({
    queryKey: ["fabRegioes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-regioes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabRegioes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-regioes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabRegioeses"] });
    },
  });
}

export function useUpdateFabRegioes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-regioes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabRegioeses"] });
      queryClient.invalidateQueries({ queryKey: ["fabRegioes", data.id] });
    },
  });
}

export function useDeleteFabRegioes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-regioes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabRegioeses"] });
    },
  });
}


