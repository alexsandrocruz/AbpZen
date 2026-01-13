import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinLancamentosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinLancamentoses(input: GetFinLancamentosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finLancamentoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-lancamentos", {
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

export function useAllFinLancamentoses() {
  return useQuery({
    queryKey: ["finLancamentoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-lancamentos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinLancamentos(id: string) {
  return useQuery({
    queryKey: ["finLancamentos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-lancamentos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinLancamentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-lancamentos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finLancamentoses"] });
    },
  });
}

export function useUpdateFinLancamentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-lancamentos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finLancamentoses"] });
      queryClient.invalidateQueries({ queryKey: ["finLancamentos", data.id] });
    },
  });
}

export function useDeleteFinLancamentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-lancamentos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finLancamentoses"] });
    },
  });
}


