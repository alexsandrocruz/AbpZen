import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliLocaisAtendidosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliLocaisAtendidos(input: GetAdvCliLocaisAtendidosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliLocaisAtendidos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-locais-atendido", {
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

export function useAllAdvCliLocaisAtendidos() {
  return useQuery({
    queryKey: ["advCliLocaisAtendidos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-locais-atendido", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliLocaisAtendido(id: string) {
  return useQuery({
    queryKey: ["advCliLocaisAtendido", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-locais-atendido/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliLocaisAtendido() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-locais-atendido", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliLocaisAtendidos"] });
    },
  });
}

export function useUpdateAdvCliLocaisAtendido() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-locais-atendido/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliLocaisAtendidos"] });
      queryClient.invalidateQueries({ queryKey: ["advCliLocaisAtendido", data.id] });
    },
  });
}

export function useDeleteAdvCliLocaisAtendido() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-locais-atendido/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliLocaisAtendidos"] });
    },
  });
}


