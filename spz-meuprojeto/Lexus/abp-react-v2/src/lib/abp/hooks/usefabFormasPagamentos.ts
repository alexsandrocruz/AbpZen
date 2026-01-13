import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabFormasPagamentosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabFormasPagamentos(input: GetFabFormasPagamentosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabFormasPagamentos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-formas-pagamento", {
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

export function useAllFabFormasPagamentos() {
  return useQuery({
    queryKey: ["fabFormasPagamentos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-formas-pagamento", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabFormasPagamento(id: string) {
  return useQuery({
    queryKey: ["fabFormasPagamento", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-formas-pagamento/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabFormasPagamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-formas-pagamento", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabFormasPagamentos"] });
    },
  });
}

export function useUpdateFabFormasPagamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-formas-pagamento/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabFormasPagamentos"] });
      queryClient.invalidateQueries({ queryKey: ["fabFormasPagamento", data.id] });
    },
  });
}

export function useDeleteFabFormasPagamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-formas-pagamento/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabFormasPagamentos"] });
    },
  });
}


