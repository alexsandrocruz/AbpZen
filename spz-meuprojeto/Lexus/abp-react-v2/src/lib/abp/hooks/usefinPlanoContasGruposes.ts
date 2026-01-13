import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinPlanoContasGruposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinPlanoContasGruposes(input: GetFinPlanoContasGruposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finPlanoContasGruposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-plano-contas-grupos", {
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

export function useAllFinPlanoContasGruposes() {
  return useQuery({
    queryKey: ["finPlanoContasGruposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-plano-contas-grupos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinPlanoContasGrupos(id: string) {
  return useQuery({
    queryKey: ["finPlanoContasGrupos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-plano-contas-grupos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinPlanoContasGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-plano-contas-grupos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasGruposes"] });
    },
  });
}

export function useUpdateFinPlanoContasGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-plano-contas-grupos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasGruposes"] });
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasGrupos", data.id] });
    },
  });
}

export function useDeleteFinPlanoContasGrupos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-plano-contas-grupos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finPlanoContasGruposes"] });
    },
  });
}


