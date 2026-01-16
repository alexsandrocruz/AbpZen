import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetUsuAcessosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useUsuAcessoses(input: GetUsuAcessosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["usuAcessoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-acessos", {
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

export function useAllUsuAcessoses() {
  return useQuery({
    queryKey: ["usuAcessoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-acessos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useUsuAcessos(id: string) {
  return useQuery({
    queryKey: ["usuAcessos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/usu-acessos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateUsuAcessos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/usu-acessos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuAcessoses"] });
    },
  });
}

export function useUpdateUsuAcessos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/usu-acessos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["usuAcessoses"] });
      queryClient.invalidateQueries({ queryKey: ["usuAcessos", data.id] });
    },
  });
}

export function useDeleteUsuAcessos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/usu-acessos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuAcessoses"] });
    },
  });
}


