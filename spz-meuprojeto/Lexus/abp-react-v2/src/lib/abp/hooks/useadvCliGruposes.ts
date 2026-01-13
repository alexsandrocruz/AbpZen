import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliGruposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliGruposes(input: GetAdvCliGruposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliGruposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-grupos", {
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

export function useAllAdvCliGruposes() {
  return useQuery({
    queryKey: ["advCliGruposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-grupos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliGrupos(id: string) {
  return useQuery({
    queryKey: ["advCliGrupos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-grupos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-grupos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliGruposes"] });
    },
  });
}

export function useUpdateAdvCliGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-grupos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliGruposes"] });
      queryClient.invalidateQueries({ queryKey: ["advCliGrupos", data.id] });
    },
  });
}

export function useDeleteAdvCliGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-grupos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliGruposes"] });
    },
  });
}


