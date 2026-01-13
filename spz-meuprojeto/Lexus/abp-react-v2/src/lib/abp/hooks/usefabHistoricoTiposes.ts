import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabHistoricoTiposesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabHistoricoTiposes(input: GetFabHistoricoTiposesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabHistoricoTiposes", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-historico-tipos", {
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

export function useAllFabHistoricoTiposes() {
  return useQuery({
    queryKey: ["fabHistoricoTiposes", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-historico-tipos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabHistoricoTipos(id: string) {
  return useQuery({
    queryKey: ["fabHistoricoTipos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-historico-tipos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabHistoricoTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-historico-tipos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabHistoricoTiposes"] });
    },
  });
}

export function useUpdateFabHistoricoTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-historico-tipos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabHistoricoTiposes"] });
      queryClient.invalidateQueries({ queryKey: ["fabHistoricoTipos", data.id] });
    },
  });
}

export function useDeleteFabHistoricoTipos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-historico-tipos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabHistoricoTiposes"] });
    },
  });
}


