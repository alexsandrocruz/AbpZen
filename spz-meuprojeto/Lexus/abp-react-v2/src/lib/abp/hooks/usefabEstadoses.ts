import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabEstadosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabEstadoses(input: GetFabEstadosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabEstadoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-estados", {
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

export function useAllFabEstadoses() {
  return useQuery({
    queryKey: ["fabEstadoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-estados", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabEstados(id: string) {
  return useQuery({
    queryKey: ["fabEstados", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-estados/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabEstados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-estados", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabEstadoses"] });
    },
  });
}

export function useUpdateFabEstados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-estados/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabEstadoses"] });
      queryClient.invalidateQueries({ queryKey: ["fabEstados", data.id] });
    },
  });
}

export function useDeleteFabEstados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-estados/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabEstadoses"] });
    },
  });
}


