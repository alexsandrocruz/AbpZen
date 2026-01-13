import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabDatasEFeriadosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabDatasEFeriadoses(input: GetFabDatasEFeriadosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabDatasEFeriadoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-datas-eferiados", {
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

export function useAllFabDatasEFeriadoses() {
  return useQuery({
    queryKey: ["fabDatasEFeriadoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-datas-eferiados", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabDatasEFeriados(id: string) {
  return useQuery({
    queryKey: ["fabDatasEFeriados", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-datas-eferiados/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabDatasEFeriados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-datas-eferiados", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabDatasEFeriadoses"] });
    },
  });
}

export function useUpdateFabDatasEFeriados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-datas-eferiados/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabDatasEFeriadoses"] });
      queryClient.invalidateQueries({ queryKey: ["fabDatasEFeriados", data.id] });
    },
  });
}

export function useDeleteFabDatasEFeriados() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-datas-eferiados/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabDatasEFeriadoses"] });
    },
  });
}


