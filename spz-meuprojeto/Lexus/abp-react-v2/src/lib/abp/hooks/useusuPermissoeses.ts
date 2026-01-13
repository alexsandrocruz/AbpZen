import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetUsuPermissoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useUsuPermissoeses(input: GetUsuPermissoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["usuPermissoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-permissoes", {
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

export function useAllUsuPermissoeses() {
  return useQuery({
    queryKey: ["usuPermissoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-permissoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useUsuPermissoes(id: string) {
  return useQuery({
    queryKey: ["usuPermissoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/usu-permissoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateUsuPermissoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/usu-permissoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuPermissoeses"] });
    },
  });
}

export function useUpdateUsuPermissoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/usu-permissoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["usuPermissoeses"] });
      queryClient.invalidateQueries({ queryKey: ["usuPermissoes", data.id] });
    },
  });
}

export function useDeleteUsuPermissoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/usu-permissoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuPermissoeses"] });
    },
  });
}


