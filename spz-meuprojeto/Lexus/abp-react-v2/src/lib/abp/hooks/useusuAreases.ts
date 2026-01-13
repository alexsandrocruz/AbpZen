import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetUsuAreasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useUsuAreases(input: GetUsuAreasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["usuAreases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-areas", {
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

export function useAllUsuAreases() {
  return useQuery({
    queryKey: ["usuAreases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/usu-areas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useUsuAreas(id: string) {
  return useQuery({
    queryKey: ["usuAreas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/usu-areas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateUsuAreas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/usu-areas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuAreases"] });
    },
  });
}

export function useUpdateUsuAreas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/usu-areas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["usuAreases"] });
      queryClient.invalidateQueries({ queryKey: ["usuAreas", data.id] });
    },
  });
}

export function useDeleteUsuAreas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/usu-areas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["usuAreases"] });
    },
  });
}


