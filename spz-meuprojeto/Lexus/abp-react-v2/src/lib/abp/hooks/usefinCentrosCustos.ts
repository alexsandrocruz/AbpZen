import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinCentrosCustosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinCentrosCustos(input: GetFinCentrosCustosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finCentrosCustos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-centros-custo", {
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

export function useAllFinCentrosCustos() {
  return useQuery({
    queryKey: ["finCentrosCustos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-centros-custo", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinCentrosCusto(id: string) {
  return useQuery({
    queryKey: ["finCentrosCusto", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-centros-custo/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinCentrosCusto() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-centros-custo", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finCentrosCustos"] });
    },
  });
}

export function useUpdateFinCentrosCusto() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-centros-custo/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finCentrosCustos"] });
      queryClient.invalidateQueries({ queryKey: ["finCentrosCusto", data.id] });
    },
  });
}

export function useDeleteFinCentrosCusto() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-centros-custo/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finCentrosCustos"] });
    },
  });
}


