import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetOpoOportunidadesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useOpoOportunidadeses(input: GetOpoOportunidadesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["opoOportunidadeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-oportunidades", {
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

export function useAllOpoOportunidadeses() {
  return useQuery({
    queryKey: ["opoOportunidadeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-oportunidades", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useOpoOportunidades(id: string) {
  return useQuery({
    queryKey: ["opoOportunidades", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/opo-oportunidades/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateOpoOportunidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/opo-oportunidades", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
    },
  });
}

export function useUpdateOpoOportunidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/opo-oportunidades/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
      queryClient.invalidateQueries({ queryKey: ["opoOportunidades", data.id] });
    },
  });
}

export function useDeleteOpoOportunidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/opo-oportunidades/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoOportunidadeses"] });
    },
  });
}


