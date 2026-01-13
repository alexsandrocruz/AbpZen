import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabPermissoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabPermissoeses(input: GetFabPermissoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabPermissoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-permissoes", {
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

export function useAllFabPermissoeses() {
  return useQuery({
    queryKey: ["fabPermissoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-permissoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabPermissoes(id: string) {
  return useQuery({
    queryKey: ["fabPermissoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-permissoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabPermissoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-permissoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabPermissoeses"] });
    },
  });
}

export function useUpdateFabPermissoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-permissoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabPermissoeses"] });
      queryClient.invalidateQueries({ queryKey: ["fabPermissoes", data.id] });
    },
  });
}

export function useDeleteFabPermissoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-permissoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabPermissoeses"] });
    },
  });
}


