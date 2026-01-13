import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProfissionaisEstadosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProfissionaisEstadoses(input: GetAdvProfissionaisEstadosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProfissionaisEstadoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-profissionais-estados", {
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

export function useAllAdvProfissionaisEstadoses() {
  return useQuery({
    queryKey: ["advProfissionaisEstadoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-profissionais-estados", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProfissionaisEstados(id: string) {
  return useQuery({
    queryKey: ["advProfissionaisEstados", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-profissionais-estados/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProfissionaisEstados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-profissionais-estados", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisEstadoses"] });
    },
  });
}

export function useUpdateAdvProfissionaisEstados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-profissionais-estados/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisEstadoses"] });
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisEstados", data.id] });
    },
  });
}

export function useDeleteAdvProfissionaisEstados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-profissionais-estados/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisEstadoses"] });
    },
  });
}


