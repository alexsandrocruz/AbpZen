import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabLembretesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabLembreteses(input: GetFabLembretesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabLembreteses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-lembretes", {
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

export function useAllFabLembreteses() {
  return useQuery({
    queryKey: ["fabLembreteses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-lembretes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabLembretes(id: string) {
  return useQuery({
    queryKey: ["fabLembretes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-lembretes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabLembretes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-lembretes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabLembreteses"] });
    },
  });
}

export function useUpdateFabLembretes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-lembretes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabLembreteses"] });
      queryClient.invalidateQueries({ queryKey: ["fabLembretes", data.id] });
    },
  });
}

export function useDeleteFabLembretes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-lembretes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabLembreteses"] });
    },
  });
}


