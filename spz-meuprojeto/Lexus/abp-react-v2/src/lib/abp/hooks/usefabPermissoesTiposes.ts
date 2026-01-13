import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabPermissoesTiposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabPermissoesTiposes(input: GetFabPermissoesTiposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabPermissoesTiposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-permissoes-tipos", {
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

export function useAllFabPermissoesTiposes() {
  return useQuery({
    queryKey: ["fabPermissoesTiposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-permissoes-tipos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabPermissoesTipos(id: string) {
  return useQuery({
    queryKey: ["fabPermissoesTipos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-permissoes-tipos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabPermissoesTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-permissoes-tipos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabPermissoesTiposes"] });
    },
  });
}

export function useUpdateFabPermissoesTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-permissoes-tipos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabPermissoesTiposes"] });
      queryClient.invalidateQueries({ queryKey: ["fabPermissoesTipos", data.id] });
    },
  });
}

export function useDeleteFabPermissoesTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-permissoes-tipos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabPermissoesTiposes"] });
    },
  });
}


