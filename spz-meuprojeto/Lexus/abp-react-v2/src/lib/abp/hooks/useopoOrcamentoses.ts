import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetOpoOrcamentosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useOpoOrcamentoses(input: GetOpoOrcamentosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["opoOrcamentoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-orcamentos", {
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

export function useAllOpoOrcamentoses() {
  return useQuery({
    queryKey: ["opoOrcamentoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-orcamentos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useOpoOrcamentos(id: string) {
  return useQuery({
    queryKey: ["opoOrcamentos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/opo-orcamentos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateOpoOrcamentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/opo-orcamentos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOrcamentoses"] });
    },
  });
}

export function useUpdateOpoOrcamentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/opo-orcamentos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["opoOrcamentoses"] });
      queryClient.invalidateQueries({ queryKey: ["opoOrcamentos", data.id] });
    },
  });
}

export function useDeleteOpoOrcamentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/opo-orcamentos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOrcamentoses"] });
    },
  });
}


