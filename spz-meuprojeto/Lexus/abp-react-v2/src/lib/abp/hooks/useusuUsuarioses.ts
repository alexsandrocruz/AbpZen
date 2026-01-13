import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetUsuUsuariosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useUsuUsuarioses(input: GetUsuUsuariosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["usuUsuarioses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-usuarios", {
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

export function useAllUsuUsuarioses() {
  return useQuery({
    queryKey: ["usuUsuarioses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-usuarios", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useUsuUsuarios(id: string) {
  return useQuery({
    queryKey: ["usuUsuarios", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/usu-usuarios/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateUsuUsuarios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/usu-usuarios", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuUsuarioses"] });
    },
  });
}

export function useUpdateUsuUsuarios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/usu-usuarios/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["usuUsuarioses"] });
      queryClient.invalidateQueries({ queryKey: ["usuUsuarios", data.id] });
    },
  });
}

export function useDeleteUsuUsuarios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/usu-usuarios/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuUsuarioses"] });
    },
  });
}


