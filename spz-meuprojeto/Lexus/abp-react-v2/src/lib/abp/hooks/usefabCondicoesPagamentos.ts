import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFabCondicoesPagamentosInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFabCondicoesPagamentos(input: GetFabCondicoesPagamentosInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["fabCondicoesPagamentos", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-condicoes-pagamento", {
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

export function useAllFabCondicoesPagamentos() {
  return useQuery({
    queryKey: ["fabCondicoesPagamentos", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fab-condicoes-pagamento", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFabCondicoesPagamento(id: string) {
  return useQuery({
    queryKey: ["fabCondicoesPagamento", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fab-condicoes-pagamento/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFabCondicoesPagamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fab-condicoes-pagamento", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabCondicoesPagamentos"] });
    },
  });
}

export function useUpdateFabCondicoesPagamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fab-condicoes-pagamento/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["fabCondicoesPagamentos"] });
      queryClient.invalidateQueries({ queryKey: ["fabCondicoesPagamento", data.id] });
    },
  });
}

export function useDeleteFabCondicoesPagamento() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fab-condicoes-pagamento/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["fabCondicoesPagamentos"] });
    },
  });
}


