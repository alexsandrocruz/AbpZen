import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabMotivosAproveitamentosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabMotivosAproveitamentos(input: GetFabMotivosAproveitamentosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabMotivosAproveitamentos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-motivos-aproveitamento", {
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

export function useAllFabMotivosAproveitamentos() {
  return useQuery({
    queryKey: ["fabMotivosAproveitamentos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-motivos-aproveitamento", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabMotivosAproveitamento(id: string) {
  return useQuery({
    queryKey: ["fabMotivosAproveitamento", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-motivos-aproveitamento/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabMotivosAproveitamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-motivos-aproveitamento", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabMotivosAproveitamentos"] });
    },
  });
}

export function useUpdateFabMotivosAproveitamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-motivos-aproveitamento/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabMotivosAproveitamentos"] });
      queryClient.invalidateQueries({ queryKey: ["fabMotivosAproveitamento", data.id] });
    },
  });
}

export function useDeleteFabMotivosAproveitamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-motivos-aproveitamento/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabMotivosAproveitamentos"] });
    },
  });
}


