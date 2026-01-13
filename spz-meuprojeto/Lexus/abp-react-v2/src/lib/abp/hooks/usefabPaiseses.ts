import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabPaisesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabPaiseses(input: GetFabPaisesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabPaiseses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-paises", {
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

export function useAllFabPaiseses() {
  return useQuery({
    queryKey: ["fabPaiseses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-paises", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabPaises(id: string) {
  return useQuery({
    queryKey: ["fabPaises", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-paises/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabPaises() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-paises", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabPaiseses"] });
    },
  });
}

export function useUpdateFabPaises() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-paises/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabPaiseses"] });
      queryClient.invalidateQueries({ queryKey: ["fabPaises", data.id] });
    },
  });
}

export function useDeleteFabPaises() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-paises/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabPaiseses"] });
    },
  });
}


